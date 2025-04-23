
// src/hooks/useBookActChapterCascade.js

import { useState, useEffect } from "react";
import { getRelatedActs, getRelatedChapters } from "../../features/plotpoints/PlotPointApi";
import { fetchBooks } from "../api/DropdownApi"; // Adjust the import path as necessary


export function useBookActChapterCascade() {
    const [books, setBooks] = useState([]);
    const [acts, setActs] = useState([]);
    const [chapters, setChapters] = useState([]);

    const [selectedBookId, setSelectedBookId] = useState('');
    const [selectedActId, setSelectedActId] = useState('');
    const [selectedChapterId, setSelectedChapterId] = useState('');

    useEffect(() => {
        async function loadBooks() {
            try {
                const bookList = await fetchBooks();
                setBooks(bookList);
            } catch (err) {
                console.error('❌ Failed to load books:', err);
            }
        }
        loadBooks();
    }, []);

    const handleBookChange = async (bookId) => {
        setSelectedBookId(bookId);
        setSelectedActId('');
        setSelectedChapterId('');
        setActs([]);
        setChapters([]);

        try {
            if (bookId) {
                const relatedActs = await getRelatedActs(bookId);
                setActs(relatedActs);
            }
        } catch (err) {
            console.error(`❌ Could not load acts for book ${bookId}`, err);
        }
    };

    const handleActChange = async (actId) => {
        setSelectedActId(actId);
        setSelectedChapterId('');
        setChapters([]);

        try {
            if (actId) {
                const relatedChapters = await getRelatedChapters(actId);
                setChapters(relatedChapters);
            }
        } catch (err) {
            console.error(`❌ Could not load chapters for act ${actId}`, err);
        }
    };

    return {
        books,
        acts,
        chapters,
        selectedBookId,
        selectedActId,
        selectedChapterId,
        setSelectedBookId,     // 🔥 required
        setSelectedActId,      // 🔥 optional but useful
        setSelectedChapterId,
        handleBookChange,
        handleActChange,
    };
}
