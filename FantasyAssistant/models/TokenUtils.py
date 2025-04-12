import re

def estimate_tokens(text):
    if not text:
        return 0
    # Rough estimation: 1 token ≈ 0.75 words
    # GPT-3.5-like models: ~4 characters per token average
    words = re.findall(r"\w+", text)
    return int(len(words) * 1.3)
