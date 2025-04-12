# config.py (in utils/)

import os

BASE_PATH = os.path.abspath(os.path.join(os.path.dirname(__file__), '..'))
SEED_DATA_PATH = os.path.join(BASE_PATH, 'FantasyDB', 'Seed Data')
SEED_FK_PATH = os.path.join(BASE_PATH, 'FantasyDB', 'Seed Data with FKs')
VECTOR_DB_PATH = "embeddings/vector_store"
OLLAMA_HOST = "http://localhost:11434"
DEFAULT_MODEL = "mistral"