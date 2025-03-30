# generation_session.py (in scenes/)

from embeddings.loader import embed_seed_data
from models.ollama_client import chat_with_ollama
from models.snapshot_prompt_engine import build_snapshot_prompt_from_plotpoint
from utils.usage_logger import log_usage

import time

def generate_scene(plotpoint_id=None):
    embed_seed_data()
    user_prompt = input("Enter your scene prompt: ")

    context_prompt = build_snapshot_prompt_from_plotpoint(plotpoint_id) if plotpoint_id else "[System] No PlotPoint context loaded."
    full_prompt = f"{context_prompt}\n\n{user_prompt}"

    start = time.time()
    result = chat_with_ollama(full_prompt)
    end = time.time()

    print("\n--- Generated Scene ---\n")
    print(result)
    log_usage(tokens_in=len(full_prompt.split()), tokens_out=len(result.split()), time_taken=end - start)