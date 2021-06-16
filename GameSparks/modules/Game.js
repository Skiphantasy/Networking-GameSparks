var createGame = function() {
    var prizeValue = 0;
    var stoppedSlot = "";
    
    var tryMove = function(y) {
        if (isPositionValid(y)) {
            return true;
        }
        else {
            return false;
        }
    };
    
    var isPositionValid = function(y) {
        return y >= -2 && y <= 3.25;
    };
    
    var getStoppedSlot = function(y) {
        if (isPositionValid) {
            if (y == -2)
                stoppedSlot = "Diamond";
            else if (y == -1.25)
                stoppedSlot = "Crown";
            else if (y == -0.5)
                stoppedSlot = "Melon";
            else if (y == 0.25)
                stoppedSlot = "Bar";
            else if (y == 1)
                stoppedSlot = "Seven";
            else if (y == 1.75)
                stoppedSlot = "Cherry";
            else if (y == 2.5)
                stoppedSlot = "Lemon";
            else if (y == 3.25)
                stoppedSlot = "Diamond";
        };
        
        return stoppedSlot;
    }
    
    var checkResults = function(row1, row2, row3) {
        if (row1 == "Diamond" && row2 == "Diamond"
            && row3 == "Diamond")
            prizeValue = 200;

        else if (row1 == "Crown" && row2 == "Crown"
            && row3 == "Crown")
            prizeValue = 400;

        else if (row1 == "Melon" && row2 == "Melon"
            && row3 == "Melon")
            prizeValue = 600;

        else if (row1 == "Bar" && row2 == "Bar"
            && row3 == "Bar")
            prizeValue = 800;

        else if (row1 == "Seven" && row2 == "Seven"
            && row3 == "Seven")
            prizeValue = 1500;

        else if (row1 == "Cherry" && row2 == "Cherry"
            && row3 == "Cherry")
            prizeValue = 3000;

        else if (row1 == "Lemon" && row2 == "Lemon"
            && row3 == "Lemon")
            prizeValue = 5000;

        return prizeValue;
    };
    
    return {
        tryMove : tryMove,
        isPositionValid : isPositionValid,
        checkResults : checkResults,
        getStoppedSlot: getStoppedSlot
    };
}