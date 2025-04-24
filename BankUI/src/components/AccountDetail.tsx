import { useEffect, useState } from "react";

interface Account {
    accountNumber: string;
    ownerName: string;
    balance: number;
}

interface Props {
    accountNumber: string;
}

export default function AccountDetail({ accountNumber }: Props) {
    const [account, setAccount] = useState<Account | null>(null);

    useEffect(() => {
        fetch(`https://localhost:5001/api/bank/${accountNumber}`)
            .then((res) => res.json())
            .then(setAccount)
            .catch(console.error);
    }, [accountNumber]);

    if (!account) return <p>Načítám údaje o účtu...</p>;

    return (
        <div>
            <h2>Účet</h2>
            <p><strong>Majitel:</strong> {account.ownerName}</p>
            <p><strong>Číslo účtu:</strong> {account.accountNumber}</p>
            <p><strong>Zůstatek:</strong> {account.balance.toFixed(2)} Kč</p>
        </div>
    );
}