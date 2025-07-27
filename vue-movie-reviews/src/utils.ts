export function formatDate(date: Date | null): string {
  if (!date) return 'N/A';
  return new Date(date).toLocaleDateString();
}

export function formatDateIfNullReturnsEmptyString(date: Date | null): string {
  if (!date) return '';

  const original = new Date(date);
  const local = new Date(original.getTime() - original.getTimezoneOffset() * 60000);

  return local.toISOString().split('T')[0];
}
