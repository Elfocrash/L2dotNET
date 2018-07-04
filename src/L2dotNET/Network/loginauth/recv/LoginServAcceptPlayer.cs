using System;
using System.Threading.Tasks;
using L2dotNET.DataContracts;
using L2dotNET.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET.Network.loginauth.recv
{
    class LoginServAcceptPlayer : PacketBase
    {
        private readonly ICrudService<AccountContract> _accountCrudService;
        private readonly AuthThread _login;
        private readonly int _accountId;
        private readonly SessionKey _key;
        public LoginServAcceptPlayer(IServiceProvider serviceProvider, Packet p, AuthThread login) : base(serviceProvider)
        {
            _accountCrudService = serviceProvider.GetService<ICrudService<AccountContract>>();
            _login = login;
            _accountId = p.ReadInt();
            _key = new SessionKey(p.ReadInt(), p.ReadInt(), p.ReadInt(), p.ReadInt());
        }

        public override async Task RunImpl()
        {
            AccountContract account = await _accountCrudService.GetById(_accountId);

            _login.AwaitAddAccount(account, _key);
        }
    }
}