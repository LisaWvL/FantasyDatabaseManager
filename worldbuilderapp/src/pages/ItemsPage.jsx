// src/pages/ItemPage.jsx
import React, { useEffect, useState } from "react";
import {
    fetchItems,
    deleteItem,
    //fetchItemChapter,
    //fetchItemDuplicate
} from '../api/ItemApi';
import EntityTable from "../components/EntityTable";
import { useNavigate } from "react-router-dom";

export default function ItemPage() {
    const [items, setItems] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        loadItems();
    }, []);

    const loadItems = async () => {
        try {
            const data = await fetchItems();
            setItems(data);
        } catch (err) {
            console.error("Failed to load items:", err);
        }
    };

    const handleDelete = async (id) => {
        if (confirm("Are you sure you want to delete this item?")) {
            await deleteItem(id);
            loadItems();
        }
    };

    const handleEdit = (id) => navigate(`/item/edit/${id}`);
    const handleChapter = async (id) => navigate(`/item/${id}/new-chapter-page`);

    return (
        <div className="container">
            <h2 className="mb-3">Items</h2>
            <EntityTable
                data={items}
                onEdit={handleEdit}
                onDelete={handleDelete}
                onChapter={handleChapter}
            />
        </div>
    );
}
