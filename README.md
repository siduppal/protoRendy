# Rendy
Quick prototype for playing around with [Mustachio](https://github.com/wildbit/mustachio/wiki) templating in Adaptive Cards. Mustachio is based on [Mustache](https://mustache.github.io/).

## Usage
Compile the project locally and then fire it up like so:

`dotnet rendy.dll --d weatherdata.json --c weathercard.json`

Here `weathercard.json` is an Adaptive Card with Mustachio style placeholders, and `weatherdata.json` is a JSON data file.

### weathercard.json - An Adaptive Card with Mustachio placeholders

```json
{
  "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
  "type": "AdaptiveCard",
  "version": "1.0",
  "speak": "The forecast for Seattle January 20 is mostly clear with a High of 51 degrees and Low of 40 degrees",
  "body": [
    {
      "type": "TextBlock",
      "text": "{{city}}",
      "size": "large",
      "isSubtle": true
    },
    {
      "type": "TextBlock",
      "text": "{{date}}",
      "spacing": "none"
    },
    {
      "type": "ColumnSet",
      "columns": [
        {
          "type": "Column",
          "width": "auto",
          "items": [
            {
              "type": "Image",
              "url": "{{imageUrl}}",
              "size": "small"
            }
          ]
        },
        {
          "type": "Column",
          "width": "auto",
          "items": [
            {
              "type": "TextBlock",
              "text": "{{temperature}}",
              "size": "extraLarge",
              "spacing": "none"
            }
          ]
        },
        {
          "type": "Column",
          "width": "stretch",
          "items": [
            {
              "type": "TextBlock",
              "text": "{{units}}",
              "weight": "bolder",
              "spacing": "small"
            }
          ]
        },
        {
          "type": "Column",
          "width": "stretch",
          "items": [
            {
              "type": "TextBlock",
              "text": "Hi {{hi}}",
              "horizontalAlignment": "left"
            },
            {
              "type": "TextBlock",
              "text": "Lo {{lo}}",
              "horizontalAlignment": "left",
              "spacing": "none"
            }
          ]
        }
      ]
    }
  ]
}
```

### weatherData.json - Data to be bound to the Adaptive Card
```json
{
  "city": "Seattle, WA",
  "date": "September 18, 7:30 AM",
  "imageUrl": "http://messagecardplayground.azurewebsites.net/assets/Mostly%20Cloudy-Square.png",
  "temperature": "42",
  "units": "°F",
  "hi": "51",
  "lo": "40"
}
```
