import { useEntityManager } from './useEntityManager';
import { fetchChapterById, updateChapter } from '../api/ChapterApi';

export function useChapterManager(chapterId) {
  return useEntityManager(fetchChapterById, updateChapter, chapterId);
}
