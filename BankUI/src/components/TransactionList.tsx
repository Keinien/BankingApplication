import { useEffect, useState } from "react";

interface Transaction {
    timestamp: string;
    amount: number;
    type: string;
    note?: string;
}

interface Props {
    accountNumber: string;
}

export default function TransactionList({ accountNumber }: Props) {
    const [transactions, setTransactions] = useState<Transaction[]>([]);

    useEffect(() => {
        fetch(`https://localhost:5001/api/bank/${accountNumber}/transactions`)
            .then((res) => res.json())
            .then(setTransactions)
            .catch(console.error);
    }, [accountNumber]);

    return (
        <div>
            <h2>Transakce</h2>
            <table>
                <thead>
                    <tr>
                        <th>Datum</th>
                        <th>Typ</th>
                        <th>Částka</th>
                        <th>Poznámka</th>
                    </tr>
                </thead>
                <tbody>
                    {transactions.map((tx, index) => (
                        <tr key={index}>
                            <td>{new Date(tx.timestamp).toLocaleString()}</td>
                            <td>{tx.type}</td>
                            <td>{tx.amount.toFixed(2)}</td>
                            <td>{tx.note ?? "-"}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}