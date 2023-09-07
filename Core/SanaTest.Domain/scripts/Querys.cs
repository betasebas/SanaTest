namespace SanaTest.Domain.scripts
{
    public static class Querys
    {
        public const string PRODUCTQUERY = "Select id, code, description, stock, image, value from products where stock > 0";
        public const string ADDORDERQUERY = "insert into orders(id, date, value, idcustomer) values (@id, @date, @value, @idcustomer)";
        public const string ADDORDERPRODUCTQUERY = "insert into orderproducts(id, idorder, idproduct, subvalue, quantity) values (@id, @idorder, @idproduct, @subvalue, @quantity)";
    }
}