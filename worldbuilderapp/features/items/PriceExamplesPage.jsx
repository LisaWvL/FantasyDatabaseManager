import { useEffect, useState } from 'react';
import {
  fetchPriceExamples,
  fetchPriceExampleById,
  createPriceExample,
  updatePriceExample,
  deletePriceExample,
} from './PriceExampleApi';

export default function PriceExamplePage() {
  const [examples, setExamples] = useState([]);
  const [selected, setSelected] = useState(null);
  const [formData, setFormData] = useState({ itemName: '', price: 0, currency: '' });

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    const result = await fetchPriceExamples();
    setExamples(result);
  };

  const handleEdit = async (id) => {
    const data = await fetchPriceExampleById(id);
    setSelected(id);
    setFormData(data);
  };

  const handleDelete = async (id) => {
    if (!confirm('Delete this price example?')) return;
    await deletePriceExample(id);
    loadData();
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (selected) {
      await updatePriceExample(selected, formData);
    } else {
      await createPriceExample(formData);
    }
    setFormData({ itemName: '', price: 0, currency: '' });
    setSelected(null);
    loadData();
  };

  return (
    <div className="card">
      <h2>Price Examples</h2>

      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Item Name"
          value={formData.itemName}
          onChange={(e) => setFormData({ ...formData, itemName: e.target.value })}
          required
        />
        <input
          type="number"
          placeholder="Price"
          value={formData.price}
          onChange={(e) => setFormData({ ...formData, price: parseFloat(e.target.value) })}
          required
        />
        <input
          type="text"
          placeholder="Currency"
          value={formData.currency}
          onChange={(e) => setFormData({ ...formData, currency: e.target.value })}
          required
        />
        <button type="submit">{selected ? 'Update' : 'Create'}</button>
      </form>

      <table className="table mt-3">
        <thead>
          <tr>
            <th>Item</th>
            <th>Price</th>
            <th>Currency</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {examples.map((ex) => (
            <tr key={ex.id}>
              <td>{ex.itemName}</td>
              <td>{ex.price}</td>
              <td>{ex.currency}</td>
              <td>
                <button onClick={() => handleEdit(ex.id)}>Edit</button>
                <button onClick={() => handleDelete(ex.id)}>Delete</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
