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
using WorldCupBet;

namespace WorldCupBet
{
    public partial class WorldCupBetService
    {
        public static UniTask<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, WorldCupBetDeployment worldCupBetDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<WorldCupBetDeployment>().SendRequestAndWaitForReceiptAsync(worldCupBetDeployment, cancellationTokenSource);
        }

        public static UniTask<string> DeployContractAsync(Nethereum.Web3.Web3 web3, WorldCupBetDeployment worldCupBetDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<WorldCupBetDeployment>().SendRequestAsync(worldCupBetDeployment);
        }

        public static async UniTask<WorldCupBetService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, WorldCupBetDeployment worldCupBetDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, worldCupBetDeployment, cancellationTokenSource);
            return new WorldCupBetService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.IWeb3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public WorldCupBetService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public WorldCupBetService(string contractAddress)
        {
            Web3 = Web3Unity.Web3Connect.Instance.Web3;
            ContractHandler = Web3.Eth.GetContractHandler(contractAddress);
        }

        public WorldCupBetService(Nethereum.Web3.IWeb3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public UniTask<string> AddMatchRequestAsync(AddMatchFunction addMatchFunction)
        {
             return ContractHandler.SendRequestAsync(addMatchFunction);
        }

        public UniTask<TransactionReceipt> AddMatchRequestAndWaitForReceiptAsync(AddMatchFunction addMatchFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addMatchFunction, cancellationToken);
        }

        public UniTask<string> AddMatchRequestAsync(string team1, string team2, BigInteger time, string level, BigInteger team1AwardRate, BigInteger team2AwardRate, BigInteger drawAwardRate)
        {
            var addMatchFunction = new AddMatchFunction();
                addMatchFunction.Team1 = team1;
                addMatchFunction.Team2 = team2;
                addMatchFunction.Time = time;
                addMatchFunction.Level = level;
                addMatchFunction.Team1AwardRate = team1AwardRate;
                addMatchFunction.Team2AwardRate = team2AwardRate;
                addMatchFunction.DrawAwardRate = drawAwardRate;
            
             return ContractHandler.SendRequestAsync(addMatchFunction);
        }

        public UniTask<TransactionReceipt> AddMatchRequestAndWaitForReceiptAsync(string team1, string team2, BigInteger time, string level, BigInteger team1AwardRate, BigInteger team2AwardRate, BigInteger drawAwardRate, CancellationTokenSource cancellationToken = null)
        {
            var addMatchFunction = new AddMatchFunction();
                addMatchFunction.Team1 = team1;
                addMatchFunction.Team2 = team2;
                addMatchFunction.Time = time;
                addMatchFunction.Level = level;
                addMatchFunction.Team1AwardRate = team1AwardRate;
                addMatchFunction.Team2AwardRate = team2AwardRate;
                addMatchFunction.DrawAwardRate = drawAwardRate;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addMatchFunction, cancellationToken);
        }

        public UniTask<string> BetRequestAsync(BetFunction betFunction)
        {
             return ContractHandler.SendRequestAsync(betFunction);
        }

        public UniTask<TransactionReceipt> BetRequestAndWaitForReceiptAsync(BetFunction betFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(betFunction, cancellationToken);
        }

        public UniTask<string> BetRequestAsync(BigInteger matchIndex, BigInteger choice, BigInteger amount)
        {
            var betFunction = new BetFunction();
                betFunction.MatchIndex = matchIndex;
                betFunction.Choice = choice;
                betFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(betFunction);
        }

        public UniTask<TransactionReceipt> BetRequestAndWaitForReceiptAsync(BigInteger matchIndex, BigInteger choice, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var betFunction = new BetFunction();
                betFunction.MatchIndex = matchIndex;
                betFunction.Choice = choice;
                betFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(betFunction, cancellationToken);
        }

        public UniTask<string> ClaimRequestAsync(ClaimFunction claimFunction)
        {
             return ContractHandler.SendRequestAsync(claimFunction);
        }

        public UniTask<TransactionReceipt> ClaimRequestAndWaitForReceiptAsync(ClaimFunction claimFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(claimFunction, cancellationToken);
        }

        public UniTask<string> ClaimRequestAsync(BigInteger matchIndex)
        {
            var claimFunction = new ClaimFunction();
                claimFunction.MatchIndex = matchIndex;
            
             return ContractHandler.SendRequestAsync(claimFunction);
        }

        public UniTask<TransactionReceipt> ClaimRequestAndWaitForReceiptAsync(BigInteger matchIndex, CancellationTokenSource cancellationToken = null)
        {
            var claimFunction = new ClaimFunction();
                claimFunction.MatchIndex = matchIndex;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(claimFunction, cancellationToken);
        }

        public UniTask<BigInteger> GetAwardAmountQueryAsync(GetAwardAmountFunction getAwardAmountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetAwardAmountFunction, BigInteger>(getAwardAmountFunction, blockParameter);
        }

        
        public UniTask<BigInteger> GetAwardAmountQueryAsync(BigInteger matchIndex, string beter, BlockParameter blockParameter = null)
        {
            var getAwardAmountFunction = new GetAwardAmountFunction();
                getAwardAmountFunction.MatchIndex = matchIndex;
                getAwardAmountFunction.Beter = beter;
            
            return ContractHandler.QueryAsync<GetAwardAmountFunction, BigInteger>(getAwardAmountFunction, blockParameter);
        }

        public UniTask<BigInteger> GetBetAmountQueryAsync(GetBetAmountFunction getBetAmountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetBetAmountFunction, BigInteger>(getBetAmountFunction, blockParameter);
        }

        
        public UniTask<BigInteger> GetBetAmountQueryAsync(BigInteger matchIndex, string beter, BlockParameter blockParameter = null)
        {
            var getBetAmountFunction = new GetBetAmountFunction();
                getBetAmountFunction.MatchIndex = matchIndex;
                getBetAmountFunction.Beter = beter;
            
            return ContractHandler.QueryAsync<GetBetAmountFunction, BigInteger>(getBetAmountFunction, blockParameter);
        }

        public UniTask<BigInteger> GetChoiceQueryAsync(GetChoiceFunction getChoiceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetChoiceFunction, BigInteger>(getChoiceFunction, blockParameter);
        }

        
        public UniTask<BigInteger> GetChoiceQueryAsync(BigInteger matchIndex, string beter, BlockParameter blockParameter = null)
        {
            var getChoiceFunction = new GetChoiceFunction();
                getChoiceFunction.MatchIndex = matchIndex;
                getChoiceFunction.Beter = beter;
            
            return ContractHandler.QueryAsync<GetChoiceFunction, BigInteger>(getChoiceFunction, blockParameter);
        }

        public UniTask<GetChoiceCountsOutputDTO> GetChoiceCountsQueryAsync(GetChoiceCountsFunction getChoiceCountsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetChoiceCountsFunction, GetChoiceCountsOutputDTO>(getChoiceCountsFunction, blockParameter);
        }

        public UniTask<GetChoiceCountsOutputDTO> GetChoiceCountsQueryAsync(BigInteger matchIndex, BlockParameter blockParameter = null)
        {
            var getChoiceCountsFunction = new GetChoiceCountsFunction();
                getChoiceCountsFunction.MatchIndex = matchIndex;
            
            return ContractHandler.QueryDeserializingToObjectAsync<GetChoiceCountsFunction, GetChoiceCountsOutputDTO>(getChoiceCountsFunction, blockParameter);
        }

        public UniTask<string> InitializeRequestAsync(InitializeFunction initializeFunction)
        {
             return ContractHandler.SendRequestAsync(initializeFunction);
        }

        public UniTask<TransactionReceipt> InitializeRequestAndWaitForReceiptAsync(InitializeFunction initializeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(initializeFunction, cancellationToken);
        }

        public UniTask<string> InitializeRequestAsync(string owner, string tokenAddress)
        {
            var initializeFunction = new InitializeFunction();
                initializeFunction.Owner = owner;
                initializeFunction.TokenAddress = tokenAddress;
            
             return ContractHandler.SendRequestAsync(initializeFunction);
        }

        public UniTask<TransactionReceipt> InitializeRequestAndWaitForReceiptAsync(string owner, string tokenAddress, CancellationTokenSource cancellationToken = null)
        {
            var initializeFunction = new InitializeFunction();
                initializeFunction.Owner = owner;
                initializeFunction.TokenAddress = tokenAddress;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(initializeFunction, cancellationToken);
        }

        public UniTask<BigInteger> MatchCountQueryAsync(MatchCountFunction matchCountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MatchCountFunction, BigInteger>(matchCountFunction, blockParameter);
        }

        
        public UniTask<BigInteger> MatchCountQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MatchCountFunction, BigInteger>(null, blockParameter);
        }

        public UniTask<MatchInfosOutputDTO> MatchInfosQueryAsync(MatchInfosFunction matchInfosFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<MatchInfosFunction, MatchInfosOutputDTO>(matchInfosFunction, blockParameter);
        }

        public UniTask<MatchInfosOutputDTO> MatchInfosQueryAsync(BigInteger returnValue1, BlockParameter blockParameter = null)
        {
            var matchInfosFunction = new MatchInfosFunction();
                matchInfosFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryDeserializingToObjectAsync<MatchInfosFunction, MatchInfosOutputDTO>(matchInfosFunction, blockParameter);
        }

        public UniTask<string> OwnerQueryAsync(OwnerFunction ownerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(ownerFunction, blockParameter);
        }

        
        public UniTask<string> OwnerQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(null, blockParameter);
        }

        public UniTask<string> RecoverTokensRequestAsync(RecoverTokensFunction recoverTokensFunction)
        {
             return ContractHandler.SendRequestAsync(recoverTokensFunction);
        }

        public UniTask<TransactionReceipt> RecoverTokensRequestAndWaitForReceiptAsync(RecoverTokensFunction recoverTokensFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(recoverTokensFunction, cancellationToken);
        }

        public UniTask<string> RecoverTokensRequestAsync(BigInteger tokenAmount)
        {
            var recoverTokensFunction = new RecoverTokensFunction();
                recoverTokensFunction.TokenAmount = tokenAmount;
            
             return ContractHandler.SendRequestAsync(recoverTokensFunction);
        }

        public UniTask<TransactionReceipt> RecoverTokensRequestAndWaitForReceiptAsync(BigInteger tokenAmount, CancellationTokenSource cancellationToken = null)
        {
            var recoverTokensFunction = new RecoverTokensFunction();
                recoverTokensFunction.TokenAmount = tokenAmount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(recoverTokensFunction, cancellationToken);
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

        public UniTask<string> SetMatchResultRequestAsync(SetMatchResultFunction setMatchResultFunction)
        {
             return ContractHandler.SendRequestAsync(setMatchResultFunction);
        }

        public UniTask<TransactionReceipt> SetMatchResultRequestAndWaitForReceiptAsync(SetMatchResultFunction setMatchResultFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setMatchResultFunction, cancellationToken);
        }

        public UniTask<string> SetMatchResultRequestAsync(BigInteger index, BigInteger result)
        {
            var setMatchResultFunction = new SetMatchResultFunction();
                setMatchResultFunction.Index = index;
                setMatchResultFunction.Result = result;
            
             return ContractHandler.SendRequestAsync(setMatchResultFunction);
        }

        public UniTask<TransactionReceipt> SetMatchResultRequestAndWaitForReceiptAsync(BigInteger index, BigInteger result, CancellationTokenSource cancellationToken = null)
        {
            var setMatchResultFunction = new SetMatchResultFunction();
                setMatchResultFunction.Index = index;
                setMatchResultFunction.Result = result;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setMatchResultFunction, cancellationToken);
        }

        public UniTask<string> TokenQueryAsync(TokenFunction tokenFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TokenFunction, string>(tokenFunction, blockParameter);
        }

        
        public UniTask<string> TokenQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TokenFunction, string>(null, blockParameter);
        }

        public UniTask<BigInteger> TotalAwardAmountQueryAsync(TotalAwardAmountFunction totalAwardAmountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalAwardAmountFunction, BigInteger>(totalAwardAmountFunction, blockParameter);
        }

        
        public UniTask<BigInteger> TotalAwardAmountQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalAwardAmountFunction, BigInteger>(null, blockParameter);
        }

        public UniTask<BigInteger> TotalBetAmountQueryAsync(TotalBetAmountFunction totalBetAmountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalBetAmountFunction, BigInteger>(totalBetAmountFunction, blockParameter);
        }

        
        public UniTask<BigInteger> TotalBetAmountQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalBetAmountFunction, BigInteger>(null, blockParameter);
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

        public UniTask<string> UpdateMatchRequestAsync(UpdateMatchFunction updateMatchFunction)
        {
             return ContractHandler.SendRequestAsync(updateMatchFunction);
        }

        public UniTask<TransactionReceipt> UpdateMatchRequestAndWaitForReceiptAsync(UpdateMatchFunction updateMatchFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(updateMatchFunction, cancellationToken);
        }

        public UniTask<string> UpdateMatchRequestAsync(BigInteger index, string team1, string team2, string level, BigInteger time, BigInteger team1AwardRate, BigInteger team2AwardRate, BigInteger drawAwardRate)
        {
            var updateMatchFunction = new UpdateMatchFunction();
                updateMatchFunction.Index = index;
                updateMatchFunction.Team1 = team1;
                updateMatchFunction.Team2 = team2;
                updateMatchFunction.Level = level;
                updateMatchFunction.Time = time;
                updateMatchFunction.Team1AwardRate = team1AwardRate;
                updateMatchFunction.Team2AwardRate = team2AwardRate;
                updateMatchFunction.DrawAwardRate = drawAwardRate;
            
             return ContractHandler.SendRequestAsync(updateMatchFunction);
        }

        public UniTask<TransactionReceipt> UpdateMatchRequestAndWaitForReceiptAsync(BigInteger index, string team1, string team2, string level, BigInteger time, BigInteger team1AwardRate, BigInteger team2AwardRate, BigInteger drawAwardRate, CancellationTokenSource cancellationToken = null)
        {
            var updateMatchFunction = new UpdateMatchFunction();
                updateMatchFunction.Index = index;
                updateMatchFunction.Team1 = team1;
                updateMatchFunction.Team2 = team2;
                updateMatchFunction.Level = level;
                updateMatchFunction.Time = time;
                updateMatchFunction.Team1AwardRate = team1AwardRate;
                updateMatchFunction.Team2AwardRate = team2AwardRate;
                updateMatchFunction.DrawAwardRate = drawAwardRate;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(updateMatchFunction, cancellationToken);
        }
    }
}
