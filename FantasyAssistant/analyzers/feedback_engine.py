# feedback_engine.py (in analyzers/)

def analyze_chapter(file_path):
    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()

    prompt = (
        "You are a literary editor. Analyze the following chapter and provide feedback on pacing, character voice, logical and context analysis, "
        "pacing of the story arc, cohesion of information and characters within the storyline, and emotional flow.\n\n" + content
    )

    from models.ollama_client import chat_with_ollama
    result = chat_with_ollama(prompt)
    print("\n--- Feedback ---\n")
    print(result)