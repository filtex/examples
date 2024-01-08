import {Filtex, Metadata} from 'filtex-ui';
import {useEffect,useState} from "react";

import './App.css';

function App() {
  const [metadata, setMetadata] = useState(new Metadata([]));
  const [theme] = useState('dark');
  const [value] = useState(null);
  const [data, setData] = useState([]);

  useEffect(() => {
    fetch('http://localhost:8080/metadata')
        .then((res) => res.json())
        .then((data) => {
          setMetadata(data);
        })
        .catch((err) => {
          console.log(err.message);
        });
  }, []);

  const handleSubmit = async (value: any) => {
    fetch('http://localhost:8080/filter/memory?type=text', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ query: value.text })
    })
    .then((res) => res.json())
    .then((data) => {
      setData(data);
    })
    .catch((err) => {
      console.log(err.message);
    });
  };

  return (
    <div className="App">
      <div style={{ width: '50%', position: "fixed", top: 25 }}>
        <Filtex
            metadata={metadata}
            themes={[]}
            theme={theme}
            mode={'text'}
            hideMenuButton={false}
            hideSubmitButton={false}
            hideResetButton={false}
            hideSwitchButton={false}
            autoSubmitEnabled={true}
            value={value}
            onSubmit={handleSubmit} />
      </div>
      <br />
      <div className={'table-container'}>
        <table>
          <thead>
          <tr>
            <td width={'25%'}>name</td>
            <td width={'25%'}>tags</td>
            <td>version</td>
            <td>status</td>
            <td>date</td>
          </tr>
          </thead>
          <tbody>
          {
            data.length > 0
              ? data.map((x: any) =>
                    <tr key={x.id}>
                      <td>{x.name}</td>
                      <td>{x.tags ? x.tags.join(', ') : '-'}</td>
                      <td>{x.version}</td>
                      <td>{x.status.toString()}</td>
                      <td>{new Date(x.createdAt).toLocaleString()}</td>
                    </tr>
                )
              : <tr>
                  <td colSpan={4} style={{height: 250, textAlign: 'center', color: '#A9A9A9'}}>
                    NO DATA
                  </td>
                </tr>
          }
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default App;
