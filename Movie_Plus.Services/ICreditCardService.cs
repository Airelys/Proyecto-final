using Movie_Plus.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Plus.Services
{
    public interface ICreditCardService
    {
        void UpdateCreditCardMoney(CreditCard creditCard);
        CreditCard GetByNumber(long number);
        void InsertCreditCard(CreditCard creditCard);
    }
}
