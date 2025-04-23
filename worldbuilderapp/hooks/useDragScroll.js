//hooks/useDragScroll.js
import { useEffect } from 'react';

export default function useDragScroll(isDragging) {
  useEffect(() => {
    if (!isDragging) return;

    const scrollMargin = 50;
    const scrollSpeed = 10;

    const handleMouseMove = (e) => {
      if (e.clientY < scrollMargin) {
        window.scrollBy(0, -scrollSpeed);
      } else if (window.innerHeight - e.clientY < scrollMargin) {
        window.scrollBy(0, scrollSpeed);
      }
    };

    const handleWheel = (e) => {
      window.scrollBy({
        top: e.deltaY,
        behavior: 'smooth',
      });
    };

    document.addEventListener('mousemove', handleMouseMove);
    document.addEventListener('wheel', handleWheel, { passive: false });

    return () => {
      document.removeEventListener('mousemove', handleMouseMove);
      document.removeEventListener('wheel', handleWheel);
    };
  }, [isDragging]);
}
