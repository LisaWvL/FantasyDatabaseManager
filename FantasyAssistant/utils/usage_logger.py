# usage_logger.py (in utils/)

def log_usage(tokens_in, tokens_out, time_taken):
    print(f"\n[Usage Log] Tokens In: {tokens_in} | Tokens Out: {tokens_out} | Time: {time_taken:.2f}s")