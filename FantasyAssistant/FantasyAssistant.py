# FantasyAssistant.py

import argparse
from scenes.generation_session import generate_scene
from analyzers.feedback_engine import analyze_chapter
from api.assistant_server import start_api_server

def main():
    parser = argparse.ArgumentParser(description="Fantasy Writing Assistant")
    parser.add_argument('--mode', choices=['scene', 'analyze', 'chat', 'server'], required=True)
    parser.add_argument('--file', help='Path to input file for analysis')
    parser.add_argument('--plotpoint', type=int, help='PlotPointId to use for context')
    args = parser.parse_args()

    if args.mode == 'scene':
        generate_scene(args.plotpoint)
    elif args.mode == 'analyze':
        if not args.file:
            print("--file is required for analyze mode")
            return
        analyze_chapter(args.file)
    elif args.mode == 'server':
        start_api_server()
    elif args.mode == 'chat':
        print("[Chat mode not yet implemented]")

if __name__ == '__main__':
    main()
