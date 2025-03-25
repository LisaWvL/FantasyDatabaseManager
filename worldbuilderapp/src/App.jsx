import { useState } from 'react'
import './styles/App.css'
import React from "react";
import SnapshotEntityView from "./pages/SnapshotEntityView.jsx";

function App() {
  const [count, setCount] = useState(0)

  return (
      <>
          <div className="App">
              <h1 className="my-4 text-center">Worldbuilder Timeline Viewer</h1>
              <SnapshotEntityView />
          </div>
      <div>
        <a href="https://vite.dev" target="_blank">
        </a>
        <a href="https://react.dev" target="_blank">
        </a>
      </div>
      <h1>Vite + React</h1>
      <div className="card">
        <button onClick={() => setCount((count) => count + 1)}>
          count is {count}
        </button>
        <p>
          Edit <code>src/App.jsx</code> and save to test HMR
        </p>
      </div>
      <p className="read-the-docs">
        Click on the Vite and React logos to learn more
      </p>
    </>
  )
}

export default App
