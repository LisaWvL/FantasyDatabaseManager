// src/features/plotpoints/usePlotPointEntities.js
import { useEffect, useState } from 'react';
import { fetchPlotPointEntities } from '../features/plotpoints/PlotPointApi';
import { EntityFetcher } from '../store/EntityManager';
import { entitySchemas } from '../store/EntitySchemas';



export function usePlotPointEntities(plotPoint, showTooltip) {
    const [relatedEntities, setRelatedEntities] = useState({});

    useEffect(() => {
        const load = async () => {
            const entities = await fetchPlotPointEntities(plotPoint.id);
            const chapterId = plotPoint.chapterId;

            const all = {};
            for (const [type] of Object.entries(entitySchemas)) {
                try {
                    const records = await EntityFetcher.fetchAll(type);
                    all[type] = records.filter((e) => e.chapterId === chapterId);
                } catch (err) {
                    console.warn(`⚠️ Failed to load ${type}`, err);
                }
            }

            setRelatedEntities({ ...entities, ...all });
        };

        if (showTooltip && Object.keys(relatedEntities).length === 0) {
            load();
        }
    }, [showTooltip, plotPoint.id, plotPoint.chapterId, relatedEntities]);

    return relatedEntities;
}
