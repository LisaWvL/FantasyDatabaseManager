# assistant_server.py (in api/)

from fastapi import FastAPI, Request
import uvicorn
import asyncio
import sys
from models.ollama_client import chat_with_ollama
from models.snapshot_prompt_engine import build_snapshot_prompt_from_plotpoint
from fastapi.middleware.cors import CORSMiddleware

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:56507"],  # your Vite dev server
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.post("/generate-scene")
async def generate_scene(request: Request):
    data = await request.json()
    prompt = data.get("prompt", "")
    plotpoint_id = data.get("plotpointId")
    context = build_snapshot_prompt_from_plotpoint(plotpoint_id) if plotpoint_id else ""
    full_prompt = context + "\n" + prompt
    result = chat_with_ollama(full_prompt)
    return {"response": result}

def start_api_server():
    print("🟢 Starting FantasyAssistant API on http://localhost:8000")
    try:
        uvicorn.run(app, host="127.0.0.1", port=8000)
    except KeyboardInterrupt:
        print("🛑 Server interrupted. Cleaning up.")
    finally:
        print("✅ Server shutdown complete.")


@app.post("/shutdown")
async def shutdown(request: Request):
    print("🛑 Shutdown requested from browser.")

    # OPTIONAL: kill any subprocesses you launched (e.g., LLM models)
    # You might track them globally and terminate them here.

    # Schedule the shutdown after a short delay to ensure response is sent
    asyncio.create_task(delayed_shutdown())
    return {"message": "Shutting down backend."}

async def delayed_shutdown():
    await asyncio.sleep(0.5)
    print("🔻 Stopping server now...")
    sys.exit(0)  # triggers uvicorn shutdown
