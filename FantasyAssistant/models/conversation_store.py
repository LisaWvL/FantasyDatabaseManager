import requests
#from .TokenUtils import estimate_tokens

API_BASE = "http://localhost:5000/api"

def fetch_conversation_history(plotpoint_id):
    try:
        response = requests.get(f"{API_BASE}/conversationturn/by-plotpoint/{plotpoint_id}")
        response.raise_for_status()
        return response.json()
    except Exception as e:
        print("⚠️ Failed to load conversation history:", e)
        return []

def log_conversation_turn(plotpoint_id, prompt, response, dan_mode=False):
    payload = {
        "plotPointId": plotpoint_id,
        "prompt": prompt,
        "response": response,
        "danMode": dan_mode
    }
    try:
        requests.post(f"{API_BASE}/conversationturn/log", json=payload)
    except Exception as e:
        print("⚠️ Failed to log conversation turn:", e)
