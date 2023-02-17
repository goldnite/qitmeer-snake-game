using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace SnakeGame
{


    public partial class SnakeGameDeployment : SnakeGameDeploymentBase
    {
        public SnakeGameDeployment() : base(BYTECODE) { }
        public SnakeGameDeployment(string byteCode) : base(byteCode) { }
    }

    public class SnakeGameDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "";
        public SnakeGameDeploymentBase() : base(BYTECODE) { }
        public SnakeGameDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class AccAwardsFunction : AccAwardsFunctionBase { }

    [Function("accAwards", "uint256")]
    public class AccAwardsFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class AccPointsFunction : AccPointsFunctionBase { }

    [Function("accPoints", "uint256")]
    public class AccPointsFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class AwardRecordsFunction : AwardRecordsFunctionBase { }

    [Function("awardRecords", "uint256")]
    public class AwardRecordsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class AwardShareFunction : AwardShareFunctionBase { }

    [Function("awardShare", "uint256")]
    public class AwardShareFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class AwardizeFunction : AwardizeFunctionBase { }

    [Function("awardize")]
    public class AwardizeFunctionBase : FunctionMessage
    {

    }

    public partial class EndGameFunction : EndGameFunctionBase { }

    [Function("endGame")]
    public class EndGameFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "point", 1)]
        public virtual BigInteger Point { get; set; }
    }

    public partial class GetAwardRecordsFunction : GetAwardRecordsFunctionBase { }

    [Function("getAwardRecords", typeof(GetAwardRecordsOutputDTO))]
    public class GetAwardRecordsFunctionBase : FunctionMessage
    {

    }

    public partial class GetAwardShareFunction : GetAwardShareFunctionBase { }

    [Function("getAwardShare", "uint256[5]")]
    public class GetAwardShareFunctionBase : FunctionMessage
    {

    }

    public partial class GetParticipantsFunction : GetParticipantsFunctionBase { }

    [Function("getParticipants", "address[]")]
    public class GetParticipantsFunctionBase : FunctionMessage
    {

    }

    public partial class GetPlayersFunction : GetPlayersFunctionBase { }

    [Function("getPlayers", "address[]")]
    public class GetPlayersFunctionBase : FunctionMessage
    {

    }

    public partial class LastPlayedTimesFunction : LastPlayedTimesFunctionBase { }

    [Function("lastPlayedTimes", "uint256")]
    public class LastPlayedTimesFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class OwnerFunction : OwnerFunctionBase { }

    [Function("owner", "address")]
    public class OwnerFunctionBase : FunctionMessage
    {

    }

    public partial class ParticipantsFunction : ParticipantsFunctionBase { }

    [Function("participants", "address")]
    public class ParticipantsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class PlayersFunction : PlayersFunctionBase { }

    [Function("players", "address")]
    public class PlayersFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class PriceFunction : PriceFunctionBase { }

    [Function("price", "uint256")]
    public class PriceFunctionBase : FunctionMessage
    {

    }

    public partial class RenounceOwnershipFunction : RenounceOwnershipFunctionBase { }

    [Function("renounceOwnership")]
    public class RenounceOwnershipFunctionBase : FunctionMessage
    {

    }

    public partial class SetAwardFunction : SetAwardFunctionBase { }

    [Function("setAward")]
    public class SetAwardFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "first", 1)]
        public virtual BigInteger First { get; set; }
        [Parameter("uint256", "second", 2)]
        public virtual BigInteger Second { get; set; }
        [Parameter("uint256", "third", 3)]
        public virtual BigInteger Third { get; set; }
        [Parameter("uint256", "fourth", 4)]
        public virtual BigInteger Fourth { get; set; }
        [Parameter("uint256", "fifth", 5)]
        public virtual BigInteger Fifth { get; set; }
    }

    public partial class SetPriceFunction : SetPriceFunctionBase { }

    [Function("setPrice")]
    public class SetPriceFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_price", 1)]
        public virtual BigInteger Price { get; set; }
    }

    public partial class SetWithdrawAddressFunction : SetWithdrawAddressFunctionBase { }

    [Function("setWithdrawAddress")]
    public class SetWithdrawAddressFunctionBase : FunctionMessage
    {
        [Parameter("address", "_withdrawAddress", 1)]
        public virtual string WithdrawAddress { get; set; }
    }

    public partial class StartGameFunction : StartGameFunctionBase { }

    [Function("startGame")]
    public class StartGameFunctionBase : FunctionMessage
    {

    }

    public partial class TotalPointsFunction : TotalPointsFunctionBase { }

    [Function("totalPoints", "uint256")]
    public class TotalPointsFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class TransferOwnershipFunction : TransferOwnershipFunctionBase { }

    [Function("transferOwnership")]
    public class TransferOwnershipFunctionBase : FunctionMessage
    {
        [Parameter("address", "newOwner", 1)]
        public virtual string NewOwner { get; set; }
    }

    public partial class WithdrawAddressFunction : WithdrawAddressFunctionBase { }

    [Function("withdrawAddress", "address")]
    public class WithdrawAddressFunctionBase : FunctionMessage
    {

    }

    public partial class AwardEventDTO : AwardEventDTOBase { }

    [Event("Award")]
    public class AwardEventDTOBase : IEventDTO
    {
        [Parameter("address", "player", 1, false )]
        public virtual string Player { get; set; }
        [Parameter("uint256", "rank", 2, false )]
        public virtual BigInteger Rank { get; set; }
        [Parameter("uint256", "amount", 3, false )]
        public virtual BigInteger Amount { get; set; }
        [Parameter("uint256", "timestamp", 4, false )]
        public virtual BigInteger Timestamp { get; set; }
    }

    public partial class EndGameEventDTO : EndGameEventDTOBase { }

    [Event("EndGame")]
    public class EndGameEventDTOBase : IEventDTO
    {
        [Parameter("address", "player", 1, false )]
        public virtual string Player { get; set; }
        [Parameter("uint256", "point", 2, false )]
        public virtual BigInteger Point { get; set; }
        [Parameter("uint256", "timestamp", 3, false )]
        public virtual BigInteger Timestamp { get; set; }
    }

    public partial class OwnershipTransferredEventDTO : OwnershipTransferredEventDTOBase { }

    [Event("OwnershipTransferred")]
    public class OwnershipTransferredEventDTOBase : IEventDTO
    {
        [Parameter("address", "previousOwner", 1, true )]
        public virtual string PreviousOwner { get; set; }
        [Parameter("address", "newOwner", 2, true )]
        public virtual string NewOwner { get; set; }
    }

    public partial class StartGameEventDTO : StartGameEventDTOBase { }

    [Event("StartGame")]
    public class StartGameEventDTOBase : IEventDTO
    {
        [Parameter("address", "player", 1, false )]
        public virtual string Player { get; set; }
        [Parameter("uint256", "timestamp", 2, false )]
        public virtual BigInteger Timestamp { get; set; }
    }

    public partial class AccAwardsOutputDTO : AccAwardsOutputDTOBase { }

    [FunctionOutput]
    public class AccAwardsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class AccPointsOutputDTO : AccPointsOutputDTOBase { }

    [FunctionOutput]
    public class AccPointsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class AwardRecordsOutputDTO : AwardRecordsOutputDTOBase { }

    [FunctionOutput]
    public class AwardRecordsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "timestamp", 1)]
        public virtual BigInteger Timestamp { get; set; }
    }

    public partial class AwardShareOutputDTO : AwardShareOutputDTOBase { }

    [FunctionOutput]
    public class AwardShareOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }





    public partial class GetAwardRecordsOutputDTO : GetAwardRecordsOutputDTOBase { }

    [FunctionOutput]
    public class GetAwardRecordsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("tuple[]", "", 1)]
        public virtual List<AwardRecord> ReturnValue1 { get; set; }
    }

    public partial class GetAwardShareOutputDTO : GetAwardShareOutputDTOBase { }

    [FunctionOutput]
    public class GetAwardShareOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256[5]", "", 1)]
        public virtual List<BigInteger> ReturnValue1 { get; set; }
    }

    public partial class GetParticipantsOutputDTO : GetParticipantsOutputDTOBase { }

    [FunctionOutput]
    public class GetParticipantsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address[]", "", 1)]
        public virtual List<string> ReturnValue1 { get; set; }
    }

    public partial class GetPlayersOutputDTO : GetPlayersOutputDTOBase { }

    [FunctionOutput]
    public class GetPlayersOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address[]", "", 1)]
        public virtual List<string> ReturnValue1 { get; set; }
    }

    public partial class LastPlayedTimesOutputDTO : LastPlayedTimesOutputDTOBase { }

    [FunctionOutput]
    public class LastPlayedTimesOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class OwnerOutputDTO : OwnerOutputDTOBase { }

    [FunctionOutput]
    public class OwnerOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class ParticipantsOutputDTO : ParticipantsOutputDTOBase { }

    [FunctionOutput]
    public class ParticipantsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class PlayersOutputDTO : PlayersOutputDTOBase { }

    [FunctionOutput]
    public class PlayersOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class PriceOutputDTO : PriceOutputDTOBase { }

    [FunctionOutput]
    public class PriceOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }











    public partial class TotalPointsOutputDTO : TotalPointsOutputDTOBase { }

    [FunctionOutput]
    public class TotalPointsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class WithdrawAddressOutputDTO : WithdrawAddressOutputDTOBase { }

    [FunctionOutput]
    public class WithdrawAddressOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }
}
