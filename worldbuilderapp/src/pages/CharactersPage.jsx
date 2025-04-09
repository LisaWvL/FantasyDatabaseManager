// src/pages/CharacterPage.jsx

import React, { useEffect, useState } from "react";
import { fetchChapters } from "../api/ChapterApi"; // Chapter API
import { fetchBooks } from "../api/BookApi"; // Book API
import { fetchActs } from "../api/ActApi"; // Act API

export default function CharacterPage() {
    const [chapters, setChapters] = useState([]);
    const [books, setBooks] = useState([]);
    const [acts, setActs] = useState([]);

    // Fetch Books, Acts, and Chapters
    useEffect(() => {
        loadBooks();
        loadActs();
        loadChapters();
    }, []);

    const loadBooks = async () => {
        try {
            const data = await fetchBooks();
            console.log('Fetched Books:', data);
            setBooks(data || []);
        } catch (err) {
            console.error("❌ Failed to load books", err);
        }
    };

    const loadActs = async () => {
        try {
            const data = await fetchActs();
            console.log('Fetched Acts:', data);
            setActs(data || []);
        } catch (err) {
            console.error("❌ Failed to load acts", err);
        }
    };

    const loadChapters = async () => {
        try {
            const data = await fetchChapters();
            console.log('Fetched Chapters:', data);
            setChapters(data || []);
        } catch (err) {
            console.error("❌ Failed to load chapters", err);
        }
    };

    return (
        <div className="character-page">
            {/* Display the chapters with Book and Act details */}
            {chapters.map((chapter) => {
                // Find the Act using ActId from the Chapter
                const act = acts.find((a) => a.id === chapter.actId);

                // Find the Book using BookId from the Act
                const book = books.find((b) => b.id === act?.bookId);

                return (
                    <div key={chapter.id} className="chapter-section">
                        {/* Book and Act details */}
                        <div className="chapter-header">
                            <h3>Book: {book?.bookNumber} - {book?.bookTitle}</h3>
                            <h3>Act: {act?.actNumber} - {act?.actTitle}</h3>
                            <h3>Chapter: {chapter.chapterNumber} - {chapter.chapterTitle}</h3>
                        </div>

                        {/* Divider Line below chapter */}
                        <hr />
                    </div>
                );
            })}
        </div>
    );
}
