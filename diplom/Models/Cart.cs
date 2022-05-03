namespace Cloth.Models
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public virtual void AddItem(Products product, int quantity, string size)
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
            else if (line.Size == size)
            {
                line.Quantity += quantity;
            }
            else
            {
                lineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity,
                    Size = size
                });
            }
        }

        public virtual void RemoveLine(Products product, string size) => lineCollection.RemoveAll(l => l.Product.Id == product.Id && l.Size == size);

        public virtual double ComputeTotalValue() => lineCollection.Sum(e => e.Product.Price * e.Quantity);

        public virtual void Clear() => lineCollection.Clear();

        public virtual IEnumerable<CartLine> Lines => lineCollection;
    }

    public class CartLine
    {
        public int CartLineId { get; set; }
        public Products Product { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
    }
}
