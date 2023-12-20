using System.ComponentModel.DataAnnotations;

namespace LoncotesLibrary.Models;

public class Patron
{
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public int CheckoutId { get; set; }
    public List<Checkout> Checkouts { get; set; }
    public bool IsActive { get; set; }

public decimal Balance 
{
    get
    {
        decimal totalBalance = 0;
        foreach (var checkout in Checkouts)
        {
            if (!checkout.Paid)
            {
                // Add the fine amount to the total balance
                totalBalance += checkout.Material.FineAmount;
            }
        }
        return totalBalance;
    } 
}

}