export function formatDate(date: Date | null): string {
  if (!date) return 'N/A';
  return new Date(date).toLocaleDateString();
}
