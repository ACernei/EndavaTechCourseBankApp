using Microsoft.AspNetCore.Identity;

namespace Endava.TechCourse.BankApp.Domain.Models;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<Wallet> Wallets { get; set; }
    public Guid? MainWalletId { get; set; }
    public Wallet? MainWallet { get; set; }
    public ICollection<Wallet> FavoriteWallets { get; set; }
}
