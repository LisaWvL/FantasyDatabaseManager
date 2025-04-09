import requests

OLLAMA_HOST = "http://localhost:11434"
MODEL_NAME = "openhermes-2.5-mistral-7b.Q5_K_M"

def chat_with_ollama(messages, model=MODEL_NAME):
    try:
        response = requests.post(f"{OLLAMA_HOST}/api/chat", json={
            "model": model,
            "messages": messages,
            "stream": False
        })
        response.raise_for_status()
        data = response.json()
        return data.get("message", {}).get("content", "[No response]")
    except Exception as e:
        raise RuntimeError(f"Failed to generate reply from Ollama: {e}")
