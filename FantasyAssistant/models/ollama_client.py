# ollama_client.py (in models/)

import requests
from utils.config import OLLAMA_HOST

def chat_with_ollama(prompt, model="mistral"):
    response = requests.post(f"{OLLAMA_HOST}/api/generate", json={
        "model": model,
        "prompt": prompt,
        "stream": False
    })
    return response.json()["response"]