#include <Adafruit_LiquidCrystal.h>

Adafruit_LiquidCrystal lcd(0);

const int rowCount = 5;
const int colCount = 5;

int rows[rowCount] = {3,4,5,6,7};
int cols[colCount] = {12,11,10,9,8};

int buzzer = 2;

bool buttonState [rowCount][colCount];
String buttonStateString = "";
String respondString = "";

void setup()
{
  Serial.begin(9600);
  lcd.begin(16, 2);
  lcd.setBacklight(HIGH);
  
  pinMode(buzzer, OUTPUT);
  
  for(int i = 0; i < rowCount; i++){
    pinMode(rows[i], OUTPUT);
  }
  
  for(int i = 0; i < colCount; i++){
    pinMode(cols[i], INPUT);
  }
  
  resetButton();
}


void loop()
{
  resetButton();
  getButtonState();
  sendButtonData();
  getRespond();
  delay(1000);
  
  
}

void getButtonState(){
  for(int i = 0; i < rowCount; i++){
    digitalWrite(rows[i],HIGH);
    	for(int j = 0; j < colCount; j++){
          if(!digitalRead(cols[j])){
          	buttonState[i][j] = 0;
          }else{
            buttonState[i][j] = 1;
          }
        }
    digitalWrite(rows[i],LOW);
  }
  
}
void resetButton(){
	for(int i = 0; i < rowCount; i++){
    	for(int j = 0; j < colCount; j++){
          buttonState[i][j] = 0;
        }
    }
}
void getRespond(){
  respondString = Serial.readString();
  if(respondString != "ALARM"){
    lcd.clear();
    lcd.print(respondString);
  }
  else
  {
    alarmMode();
  }
  
  
}

void alarmMode(){
  digitalWrite(buzzer, HIGH);
  lcd.clear();
  lcd.print("ALARM");
  delay(500);
  digitalWrite(buzzer, LOW);
}
void sendButtonData(){
  for(int i = 0; i < rowCount; i++){
    	for(int j = 0; j < colCount; j++){
          buttonStateString += buttonState[i][j];
        }
    }
  Serial.print(buttonStateString);
  buttonStateString = "";
}



  