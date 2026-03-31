export function LoadingState() { return <div className="card">Loading...</div>; }
export function ErrorState({ message }: { message?: string }) { return <div className="card">Error: {message ?? 'Something went wrong'}</div>; }
export function EmptyState({ label }: { label: string }) { return <div className="card">{label}</div>; }
