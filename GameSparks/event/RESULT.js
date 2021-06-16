require("Game");

var row1 = Spark.getData().Row1;
var row2 = Spark.getData().Row2;
var row3 = Spark.getData().Row3;

var game = createGame();
Spark.setScriptData("prizeValue", game.checkResults(row1, row2, row3))
