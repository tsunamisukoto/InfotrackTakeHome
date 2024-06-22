
Hi there,

Hope this finds you well,
I have taken the liberty of doing some unit tests, they are by no means comprehensive but for ~2 hours fiddling I think they'll suffice :)
I've left some comments scattered throughout but I'll make some notes here.
As per the spec I've used a data store of basically just a static var. In prod this would be replaced by an actual data store.
I've added an endpoint to see all the current settlement bookings in the "store"
Validation I'd probably move to a fluentvalidation or some equivalent validation layer, but as the requirements were basic, I kept the layers basic.
I've tried to keep things demonstrative of what I think you'd be looking for/at, but by no means is this fully production ready. I've done some pieces to show:
1. Never trust user input, validate validate, validate
2. splitting service layer/"entities"(SettlementBooking) from domain/contractual logic (SettlementBookingModel) and map accordingly. These are kept in line atm, but you would in a prod system obviously expose a lesser model compared to a full model.
3. I probably could have logged more, but as its quite a basic problem space I kept it to just errors.
4. Probably also could have done the validation as throwing exceptions that could get handled/logged at the base api level, but not sure which pattern I prefer.
5. I take the hours and minutes and shove it into a date object for some easy comparisons, there's probably a simpler way to do it, but I decided this was the quickest way to get the "1 hour" comparison going :P

I have attached screen grabs of the end points you'll be expected to hit. But if you download the source and run the InfotrackTakeHome project, you should have all the pieces you'll need.
![image](https://github.com/tsunamisukoto/InfotrackTakeHome/assets/11450584/99fba959-44c2-4aaa-8140-e27c2705ab52)
![image](https://github.com/tsunamisukoto/InfotrackTakeHome/assets/11450584/706e8795-9224-4405-91db-3ae206a599a2)
![image](https://github.com/tsunamisukoto/InfotrackTakeHome/assets/11450584/d5b861d8-fa7b-4a2c-a43d-4cf92e427fab)
