using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using SnakeGame;

namespace SnakeGame
{
    public partial class SnakeGameService
    {
        public static UniTask<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, SnakeGameDeployment snakeGameDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<SnakeGameDeployment>().SendRequestAndWaitForReceiptAsync(snakeGameDeployment, cancellationTokenSource);
        }

        public static UniTask<string> DeployContractAsync(Nethereum.Web3.Web3 web3, SnakeGameDeployment snakeGameDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<SnakeGameDeployment>().SendRequestAsync(snakeGameDeployment);
        }

        public static async UniTask<SnakeGameService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, SnakeGameDeployment snakeGameDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, snakeGameDeployment, cancellationTokenSource);
            return new SnakeGameService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.IWeb3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public SnakeGameService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public SnakeGameService(string contractAddress)
        {
            Web3 = Web3Unity.Web3Connect.Instance.Web3;
            ContractHandler = Web3.Eth.GetContractHandler(contractAddress);
        }

        public SnakeGameService(Nethereum.Web3.IWeb3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public UniTask<BigInteger> AccAwardsQueryAsync(AccAwardsFunction accAwardsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AccAwardsFunction, BigInteger>(accAwardsFunction, blockParameter);
        }

        
        public UniTask<BigInteger> AccAwardsQueryAsync(string returnValue1, BlockParameter blockParameter = null)
        {
            var accAwardsFunction = new AccAwardsFunction();
                accAwardsFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<AccAwardsFunction, BigInteger>(accAwardsFunction, blockParameter);
        }

        public UniTask<BigInteger> AccPointsQueryAsync(AccPointsFunction accPointsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AccPointsFunction, BigInteger>(accPointsFunction, blockParameter);
        }

        
        public UniTask<BigInteger> AccPointsQueryAsync(string returnValue1, BlockParameter blockParameter = null)
        {
            var accPointsFunction = new AccPointsFunction();
                accPointsFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<AccPointsFunction, BigInteger>(accPointsFunction, blockParameter);
        }

        public UniTask<BigInteger> AwardCountQueryAsync(AwardCountFunction awardCountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AwardCountFunction, BigInteger>(awardCountFunction, blockParameter);
        }

        
        public UniTask<BigInteger> AwardCountQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AwardCountFunction, BigInteger>(null, blockParameter);
        }

        public UniTask<BigInteger> AwardRecordsQueryAsync(AwardRecordsFunction awardRecordsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AwardRecordsFunction, BigInteger>(awardRecordsFunction, blockParameter);
        }

        
        public UniTask<BigInteger> AwardRecordsQueryAsync(BigInteger returnValue1, BlockParameter blockParameter = null)
        {
            var awardRecordsFunction = new AwardRecordsFunction();
                awardRecordsFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<AwardRecordsFunction, BigInteger>(awardRecordsFunction, blockParameter);
        }

        public UniTask<BigInteger> AwardShareQueryAsync(AwardShareFunction awardShareFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AwardShareFunction, BigInteger>(awardShareFunction, blockParameter);
        }

        
        public UniTask<BigInteger> AwardShareQueryAsync(BigInteger returnValue1, BlockParameter blockParameter = null)
        {
            var awardShareFunction = new AwardShareFunction();
                awardShareFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<AwardShareFunction, BigInteger>(awardShareFunction, blockParameter);
        }

        public UniTask<string> AwardizeRequestAsync(AwardizeFunction awardizeFunction)
        {
             return ContractHandler.SendRequestAsync(awardizeFunction);
        }

        public UniTask<string> AwardizeRequestAsync()
        {
             return ContractHandler.SendRequestAsync<AwardizeFunction>();
        }

        public UniTask<TransactionReceipt> AwardizeRequestAndWaitForReceiptAsync(AwardizeFunction awardizeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(awardizeFunction, cancellationToken);
        }

        public UniTask<TransactionReceipt> AwardizeRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<AwardizeFunction>(null, cancellationToken);
        }

        public UniTask<string> EndGameRequestAsync(EndGameFunction endGameFunction)
        {
             return ContractHandler.SendRequestAsync(endGameFunction);
        }

        public UniTask<TransactionReceipt> EndGameRequestAndWaitForReceiptAsync(EndGameFunction endGameFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(endGameFunction, cancellationToken);
        }

        public UniTask<string> EndGameRequestAsync(BigInteger point)
        {
            var endGameFunction = new EndGameFunction();
                endGameFunction.Point = point;
            
             return ContractHandler.SendRequestAsync(endGameFunction);
        }

        public UniTask<TransactionReceipt> EndGameRequestAndWaitForReceiptAsync(BigInteger point, CancellationTokenSource cancellationToken = null)
        {
            var endGameFunction = new EndGameFunction();
                endGameFunction.Point = point;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(endGameFunction, cancellationToken);
        }

        public UniTask<BigInteger> LastPlayedTimesQueryAsync(LastPlayedTimesFunction lastPlayedTimesFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<LastPlayedTimesFunction, BigInteger>(lastPlayedTimesFunction, blockParameter);
        }

        
        public UniTask<BigInteger> LastPlayedTimesQueryAsync(string returnValue1, BlockParameter blockParameter = null)
        {
            var lastPlayedTimesFunction = new LastPlayedTimesFunction();
                lastPlayedTimesFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<LastPlayedTimesFunction, BigInteger>(lastPlayedTimesFunction, blockParameter);
        }

        public UniTask<string> OwnerQueryAsync(OwnerFunction ownerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(ownerFunction, blockParameter);
        }

        
        public UniTask<string> OwnerQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(null, blockParameter);
        }

        public UniTask<BigInteger> ParticipantCountQueryAsync(ParticipantCountFunction participantCountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ParticipantCountFunction, BigInteger>(participantCountFunction, blockParameter);
        }

        
        public UniTask<BigInteger> ParticipantCountQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ParticipantCountFunction, BigInteger>(null, blockParameter);
        }

        public UniTask<string> ParticipantsQueryAsync(ParticipantsFunction participantsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ParticipantsFunction, string>(participantsFunction, blockParameter);
        }

        
        public UniTask<string> ParticipantsQueryAsync(BigInteger returnValue1, BlockParameter blockParameter = null)
        {
            var participantsFunction = new ParticipantsFunction();
                participantsFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<ParticipantsFunction, string>(participantsFunction, blockParameter);
        }

        public UniTask<BigInteger> PlayerCountQueryAsync(PlayerCountFunction playerCountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PlayerCountFunction, BigInteger>(playerCountFunction, blockParameter);
        }

        
        public UniTask<BigInteger> PlayerCountQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PlayerCountFunction, BigInteger>(null, blockParameter);
        }

        public UniTask<string> PlayersQueryAsync(PlayersFunction playersFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PlayersFunction, string>(playersFunction, blockParameter);
        }

        
        public UniTask<string> PlayersQueryAsync(BigInteger returnValue1, BlockParameter blockParameter = null)
        {
            var playersFunction = new PlayersFunction();
                playersFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<PlayersFunction, string>(playersFunction, blockParameter);
        }

        public UniTask<BigInteger> PriceQueryAsync(PriceFunction priceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PriceFunction, BigInteger>(priceFunction, blockParameter);
        }

        
        public UniTask<BigInteger> PriceQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PriceFunction, BigInteger>(null, blockParameter);
        }

        public UniTask<string> RenounceOwnershipRequestAsync(RenounceOwnershipFunction renounceOwnershipFunction)
        {
             return ContractHandler.SendRequestAsync(renounceOwnershipFunction);
        }

        public UniTask<string> RenounceOwnershipRequestAsync()
        {
             return ContractHandler.SendRequestAsync<RenounceOwnershipFunction>();
        }

        public UniTask<TransactionReceipt> RenounceOwnershipRequestAndWaitForReceiptAsync(RenounceOwnershipFunction renounceOwnershipFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(renounceOwnershipFunction, cancellationToken);
        }

        public UniTask<TransactionReceipt> RenounceOwnershipRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<RenounceOwnershipFunction>(null, cancellationToken);
        }

        public UniTask<string> SetAwardRequestAsync(SetAwardFunction setAwardFunction)
        {
             return ContractHandler.SendRequestAsync(setAwardFunction);
        }

        public UniTask<TransactionReceipt> SetAwardRequestAndWaitForReceiptAsync(SetAwardFunction setAwardFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setAwardFunction, cancellationToken);
        }

        public UniTask<string> SetAwardRequestAsync(BigInteger first, BigInteger second, BigInteger third, BigInteger fourth, BigInteger fifth)
        {
            var setAwardFunction = new SetAwardFunction();
                setAwardFunction.First = first;
                setAwardFunction.Second = second;
                setAwardFunction.Third = third;
                setAwardFunction.Fourth = fourth;
                setAwardFunction.Fifth = fifth;
            
             return ContractHandler.SendRequestAsync(setAwardFunction);
        }

        public UniTask<TransactionReceipt> SetAwardRequestAndWaitForReceiptAsync(BigInteger first, BigInteger second, BigInteger third, BigInteger fourth, BigInteger fifth, CancellationTokenSource cancellationToken = null)
        {
            var setAwardFunction = new SetAwardFunction();
                setAwardFunction.First = first;
                setAwardFunction.Second = second;
                setAwardFunction.Third = third;
                setAwardFunction.Fourth = fourth;
                setAwardFunction.Fifth = fifth;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setAwardFunction, cancellationToken);
        }

        public UniTask<string> SetPriceRequestAsync(SetPriceFunction setPriceFunction)
        {
             return ContractHandler.SendRequestAsync(setPriceFunction);
        }

        public UniTask<TransactionReceipt> SetPriceRequestAndWaitForReceiptAsync(SetPriceFunction setPriceFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setPriceFunction, cancellationToken);
        }

        public UniTask<string> SetPriceRequestAsync(BigInteger price)
        {
            var setPriceFunction = new SetPriceFunction();
                setPriceFunction.Price = price;
            
             return ContractHandler.SendRequestAsync(setPriceFunction);
        }

        public UniTask<TransactionReceipt> SetPriceRequestAndWaitForReceiptAsync(BigInteger price, CancellationTokenSource cancellationToken = null)
        {
            var setPriceFunction = new SetPriceFunction();
                setPriceFunction.Price = price;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setPriceFunction, cancellationToken);
        }

        public UniTask<string> SetWithdrawAddressRequestAsync(SetWithdrawAddressFunction setWithdrawAddressFunction)
        {
             return ContractHandler.SendRequestAsync(setWithdrawAddressFunction);
        }

        public UniTask<TransactionReceipt> SetWithdrawAddressRequestAndWaitForReceiptAsync(SetWithdrawAddressFunction setWithdrawAddressFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setWithdrawAddressFunction, cancellationToken);
        }

        public UniTask<string> SetWithdrawAddressRequestAsync(string withdrawAddress)
        {
            var setWithdrawAddressFunction = new SetWithdrawAddressFunction();
                setWithdrawAddressFunction.WithdrawAddress = withdrawAddress;
            
             return ContractHandler.SendRequestAsync(setWithdrawAddressFunction);
        }

        public UniTask<TransactionReceipt> SetWithdrawAddressRequestAndWaitForReceiptAsync(string withdrawAddress, CancellationTokenSource cancellationToken = null)
        {
            var setWithdrawAddressFunction = new SetWithdrawAddressFunction();
                setWithdrawAddressFunction.WithdrawAddress = withdrawAddress;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setWithdrawAddressFunction, cancellationToken);
        }

        public UniTask<string> StartGameRequestAsync(StartGameFunction startGameFunction)
        {
             return ContractHandler.SendRequestAsync(startGameFunction);
        }

        public UniTask<string> StartGameRequestAsync()
        {
             return ContractHandler.SendRequestAsync<StartGameFunction>();
        }

        public UniTask<TransactionReceipt> StartGameRequestAndWaitForReceiptAsync(StartGameFunction startGameFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(startGameFunction, cancellationToken);
        }

        public UniTask<TransactionReceipt> StartGameRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<StartGameFunction>(null, cancellationToken);
        }

        public UniTask<BigInteger> TotalPointsQueryAsync(TotalPointsFunction totalPointsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalPointsFunction, BigInteger>(totalPointsFunction, blockParameter);
        }

        
        public UniTask<BigInteger> TotalPointsQueryAsync(string returnValue1, BlockParameter blockParameter = null)
        {
            var totalPointsFunction = new TotalPointsFunction();
                totalPointsFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<TotalPointsFunction, BigInteger>(totalPointsFunction, blockParameter);
        }

        public UniTask<string> TransferOwnershipRequestAsync(TransferOwnershipFunction transferOwnershipFunction)
        {
             return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public UniTask<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(TransferOwnershipFunction transferOwnershipFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
        }

        public UniTask<string> TransferOwnershipRequestAsync(string newOwner)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
                transferOwnershipFunction.NewOwner = newOwner;
            
             return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public UniTask<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(string newOwner, CancellationTokenSource cancellationToken = null)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
                transferOwnershipFunction.NewOwner = newOwner;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
        }

        public UniTask<string> WithdrawAddressQueryAsync(WithdrawAddressFunction withdrawAddressFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<WithdrawAddressFunction, string>(withdrawAddressFunction, blockParameter);
        }

        
        public UniTask<string> WithdrawAddressQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<WithdrawAddressFunction, string>(null, blockParameter);
        }
    }
}
