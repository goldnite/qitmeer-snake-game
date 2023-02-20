import { ethers } from "ethers";
import { default as dotenv } from "dotenv";
import abi from "./SnakeGame.abi.json" assert { type: "json" };
dotenv.config();

const awardize = async () => {
  try {
    console.log(`Start awardize...`);
    const provider = new ethers.providers.JsonRpcProvider(
      "https://meer.testnet.meerfans.club"
    );

    const mnemonic = ethers.Wallet.fromMnemonic(process.env.WALLET_MNEMONIC);
    const wallet = new ethers.Wallet(mnemonic.privateKey, provider);

    const SnakeGame = new ethers.Contract(
      process.env.CONTRACT_ADDRESS,
      abi,
      wallet
    );

    const accountAddress = wallet.getAddress();

    const beforeEthers = Number(
      ethers.utils.formatEther(await provider.getBalance(accountAddress))
    );
    console.log("Account MEER Balance: >> ", beforeEthers);

    const tx = await SnakeGame.awardize();
    const confirmedTx = await tx.wait();
    console.log("Transaction hash:>> ", confirmedTx.hash);

    const afterEthers = Number(
      ethers.utils.formatEther(await provider.getBalance(accountAddress))
    );
    console.log("Account MEER Balance: >> ", afterEthers);
    console.log("MEERs consumed :>> ", beforeEthers - afterEthers);
  } catch (err) {
    console.error("Something went wrong");
    console.error(err);
  }
};

// invoke awardize() every 30 minutes
awardize();
setInterval(() => {
  awardize();
}, 30 * 60 * 1000);
