require("Game");

var y = Spark.getData().Y;

var game = createGame();
Spark.setScriptData("stoppedSlot", game.getStoppedSlot(y));
    