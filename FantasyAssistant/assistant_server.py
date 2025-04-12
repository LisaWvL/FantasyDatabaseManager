# assistant_server.py

from fastapi import FastAPI, Request
import uvicorn
import asyncio
import sys
from fastapi.middleware.cors import CORSMiddleware

from models.ollama_client import chat_with_ollama
from models.conversation_store import fetch_conversation_history, log_conversation_turn
from models.chapter_prompt_engine import build_chapter_prompt_from_plotpoint
from models.MemoryManager import get_turns_for_context

from pydantic import BaseModel
from typing import List, Optional

class Message(BaseModel):
    role: str
    content: str

class GenerateSceneRequest(BaseModel):
    plotpointId: int
    history: List[Message]
    danMode: Optional[bool] = False

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.get("/health")
async def health_check():
    return {"status": "ok"}

@app.post("/chat")
async def chat(request: Request):
    data = await request.json()
    plotpoint_id = data.get("plotpointId")
    history = data.get("history", [])
    dan_mode = data.get("danMode", False)

    if not plotpoint_id:
        return {"error": "Missing plotpointId"}

    chapter_prompt = build_chapter_prompt_from_plotpoint(plotpoint_id, dan_mode)
    prior_turns = fetch_conversation_history(plotpoint_id)
    smart_context = get_turns_for_context(prior_turns, chapter_prompt)

    if history and history[-1]["role"] == "user":
        latest_user_prompt = history[-1]["content"]
        smart_context.append({"role": "user", "content": latest_user_prompt})
    else:
        latest_user_prompt = ""

    try:
        assistant_reply = chat_with_ollama(smart_context)
        log_conversation_turn(plotpoint_id, latest_user_prompt, assistant_reply, dan_mode)
    except Exception as e:
        return {"error": str(e)}

    return {
        "reply": assistant_reply,
        "turn": {
            "userPrompt": latest_user_prompt,
            "assistantResponse": assistant_reply
        }
    }

@app.post("/shutdown")
async def shutdown(_: Request):
    print("\n🛑 Shutdown requested from browser.")
    asyncio.create_task(delayed_shutdown())
    return {"message": "Shutting down backend."}

async def delayed_shutdown():
    await asyncio.sleep(1)
    print("🔻 Stopping server now...")
    sys.exit(0)

def start_api_server():
    print("🟢 Starting FantasyAssistant API on http://localhost:8000")
    try:
        uvicorn.run(app, host="127.0.0.1", port=8000, log_level="info")
    except KeyboardInterrupt:
        print("🛑 Server interrupted. Cleaning up.")
    finally:
        print("✅ Server shutdown complete.")

# ✅ This ensures the API starts when run manually:
if __name__ == "__main__":
    start_api_server()
