import { ReactNode } from 'react';
import { Link } from 'react-router-dom';

export function PageHeader({ title, description, actionLabel, actionTo }: { title: string; description?: string; actionLabel?: string; actionTo?: string }) {
  return <div className="page-header">
    <div>
      <h2>{title}</h2>
      {description && <p className="muted">{description}</p>}
    </div>
    {actionLabel && actionTo && <Link className="button primary" to={actionTo}>{actionLabel}</Link>}
  </div>;
}

export function FormCard({ title, children }: { title: string; children: ReactNode }) {
  return <div className="card form-grid">
    <h3>{title}</h3>
    {children}
  </div>;
}

export function StatusBadge({ active }: { active: boolean }) {
  return <span className={active ? 'badge badge-success' : 'badge badge-muted'}>{active ? 'Active' : 'Inactive'}</span>;
}

export function ConfirmDialog({ title, message, onConfirm, confirmLabel = 'Confirm' }: { title: string; message: string; onConfirm: () => Promise<void> | void; confirmLabel?: string }) {
  return <button
    className="button danger"
    onClick={async () => {
      if (!window.confirm(`${title}\n\n${message}`)) return;
      await onConfirm();
    }}
  >
    {confirmLabel}
  </button>;
}

export function DataTable({ columns, children }: { columns: string[]; children: ReactNode }) {
  return <table className="table">
    <thead>
      <tr>{columns.map((c) => <th key={c}>{c}</th>)}</tr>
    </thead>
    <tbody>{children}</tbody>
  </table>;
}
