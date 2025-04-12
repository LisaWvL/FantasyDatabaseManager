import time
import requests
from embeddings.loader import embed_seed_data
from models.ollama_client import chat_with_ollama
from models.chapter_prompt_engine import build_chapter_prompt_from_plotpoint
from utils.usage_logger import log_usage

API_BASE = "http://localhost:5000/api"

conversation_history = []

def log_conversation_turn(plotpoint_id, prompt, response, dan_mode=False):
    payload = {
        "plotPointId": plotpoint_id,
        "prompt": prompt,
        "response": response,
        "danMode": dan_mode,
    }
    try:
        requests.post(f"{API_BASE}/conversationturn/log", json=payload)
    except Exception as e:
        print("⚠️ Failed to log turn:", e)

def generate_scene(plotpoint_id=None, dan_mode=False):
    embed_seed_data()
    chapter_context = build_chapter_prompt_from_plotpoint(plotpoint_id, dan_mode) if plotpoint_id else "[System] No PlotPoint context loaded."

    while True:
        user_input = input("You > ").strip()
        if not user_input:
            break

        conversation_history.append({ "role": "user", "content": user_input })

        messages = [
            { "role": "system", "content": chapter_context },
            *conversation_history
        ]

        start = time.time()
        response = chat_with_ollama(messages)
        end = time.time()

        print(f"\nAssistant >\n{response}\n")

        log_conversation_turn(plotpoint_id, user_input, response, dan_mode)
        conversation_history.append({ "role": "assistant", "content": response })

        log_usage(
            tokens_in=sum(len(m["content"].split()) for m in messages),
            tokens_out=len(response.split()),
            time_taken=end - start
        )
