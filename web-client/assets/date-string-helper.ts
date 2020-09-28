export default class DateStringHelper {
  private monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
  ];

  public convertToDayMonth(date:Date):string{
    let days = String(date.getDate()).padStart(2, '0');
    let month = date.getMonth();

    return "Send: " + days + " " + this.monthNames[month];
  }
}
