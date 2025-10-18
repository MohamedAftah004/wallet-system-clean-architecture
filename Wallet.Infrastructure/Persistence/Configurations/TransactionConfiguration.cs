using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet.Domain.Entities;

namespace Wallet.Infrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.WalletId)
                   .IsRequired();

            builder.Property(t => t.Type)
                   .IsRequired();

            builder.Property(t => t.Status)
                   .IsRequired();

            builder.Property(t => t.CreatedAt)
                   .IsRequired();

            // 🟢 Owned type configuration for Money
            builder.OwnsOne(t => t.Amount, amount =>
            {
                amount.Property(a => a.Amount)
                      .HasColumnName("Amount_Value")
                      .IsRequired();

                amount.OwnsOne(a => a.Currency, currency =>
                {
                    currency.Property(c => c.Code)
                            .HasColumnName("Amount_CurrencyCode")
                            .HasMaxLength(3)
                            .IsRequired();

                    currency.Property(c => c.Symbol)
                            .HasColumnName("Amount_CurrencySymbol")
                            .HasMaxLength(5)
                            .IsRequired(false);
                });
            });

            // 🟢 Owned type configuration for Transaction.Currency (إن وجدت)
            builder.OwnsOne(t => t.Currency, currency =>
            {
                currency.Property(c => c.Code)
                        .HasColumnName("Currency_Code")
                        .HasMaxLength(3)
                        .IsRequired();

                currency.Property(c => c.Symbol)
                        .HasColumnName("Currency_Symbol")
                        .HasMaxLength(5)
                        .IsRequired(false);
            });
        }
    }
}
