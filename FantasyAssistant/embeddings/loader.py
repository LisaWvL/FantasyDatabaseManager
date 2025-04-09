# embeddings/loader.py

import requests
import json
from sentence_transformers import SentenceTransformer
import chromadb
from utils import config

model = SentenceTransformer('all-MiniLM-L6-v2')
chroma_client = chromadb.PersistentClient(path=config.VECTOR_DB_PATH)
collection = chroma_client.get_or_create_collection(name="fantasy_lore")

API_BASE = "http://localhost:5000/api"

def embed_seed_data(plotpoint_id):
    print(f"[Embedding] Fetching chapter-linked data for PlotPoint {plotpoint_id}...")

    # Step 1: Get all connected chapter data
    chapter_page = requests.get(f"{API_BASE}/plotpoint/{plotpoint_id}/new-chapter-page")
    if not chapter_page.ok:
        print(f"[Error] Could not fetch chapter data for plotpoint {plotpoint_id}")
        return

    data = chapter_page.json()
    chapter = data.get("newChapter", {})

    embedded_count = 0

    # Step 2: Embed each entity in chapter
    for entity_type, entity_data in chapter.items():
        if isinstance(entity_data, dict):
            # single entity (like one POVCharacter)
            label = entity_data.get("Name") or entity_data.get("Title") or f"Unnamed {entity_type}"
            text = f"{entity_type}:\n{json.dumps(entity_data, indent=2, ensure_ascii=False)}"
            embedding = model.encode(text).tolist()
            collection.add(
                documents=[text],
                embeddings=[embedding],
                ids=[f"{entity_type}_{entity_data.get('Id', embedded_count)}"]
            )
            embedded_count += 1
        elif isinstance(entity_data, list):
            # list of entities
            for ent in entity_data:
                label = ent.get("Name") or ent.get("Title") or ent.get("Alias") or f"Unnamed {entity_type}"
                text = f"{entity_type} Entry:\n{json.dumps(ent, indent=2, ensure_ascii=False)}"
                embedding = model.encode(text).tolist()
                collection.add(
                    documents=[text],
                    embeddings=[embedding],
                    ids=[f"{entity_type}_{ent.get('Id', embedded_count)}"]
                )
                embedded_count += 1

    chroma_client.persist()
    print(f"[Embedding] Added {embedded_count} embedded entries to ChromaDB for plotpoint {plotpoint_id}.")
