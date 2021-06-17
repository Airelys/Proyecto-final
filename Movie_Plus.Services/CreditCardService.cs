using Movie_Plus.Data;
using Movie_Plus.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movie_Plus.Services
{
    public class CreditCardService : ICreditCardService
    {
        private IRepository<CreditCard> _CreditCardRepository;

        public CreditCardService(IRepository<CreditCard> CreditCardRepository)
        {
            _CreditCardRepository = CreditCardRepository;
        }

        public CreditCard GetByNumber(long number)
        {
            return _CreditCardRepository.GetAll().FirstOrDefault(c => c.Number == number);
        }

        public void InsertCreditCard(CreditCard creditCard)
        {
            _CreditCardRepository.Insert(creditCard);
        }

        public void UpdateCreditCardMoney(CreditCard creditCard)
        {
            _CreditCardRepository.Update(creditCard);
        }
    }
}
