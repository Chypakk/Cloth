namespace Cloth.Models
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public virtual void AddItem(Products product, int quantity, int size)
        {
            CartLine line = lineCollection
                .Where(p => p.Product.Id == product.Id)
                .FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity,
                    Size = size
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Products product) => lineCollection.RemoveAll(l => l.Product.Id == product.Id);

        public virtual decimal ComputeTotalValue() => lineCollection.Sum(e => e.Product.Price * e.Quantity);

        public virtual void Clear() => lineCollection.Clear();

        public virtual IEnumerable<CartLine> Lines => lineCollection;
    }

    public class CartLine
    {
        public int CartLineId { get; set; }
        public Products Product { get; set; }
        public int Quantity { get; set; }
        public int Size { get; set; }
    }
}
