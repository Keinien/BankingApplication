import AccountDetail from "./components/AccountDetail";
import TransactionList from "./components/TransactionList";
import "./App.css";

const accountNumber = "1234567890"; // nahraď číslem existujícího účtu

function App() {
    return (
        <div className="container">
            <div className="left-panel">
                <AccountDetail accountNumber={accountNumber} />
            </div>
            <div className="right-panel">
                <TransactionList accountNumber={accountNumber} />
            </div>
        </div>
    );
}

export default App;