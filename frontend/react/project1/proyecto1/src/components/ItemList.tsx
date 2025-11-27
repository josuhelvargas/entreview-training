import Item from "./Item"

export default function ItemList() {
    const items = [
        { id: 1, name: 'Item 1' },
        { id: 2, name: 'Item 2' },
        { id: 3, name: 'Item 3' },
    ]
  
  
  
  
  return (
        <div>
            {items.map(item => (
                <Item key={item.id} name={item.name} />
            ))}
        </div>
  )
}