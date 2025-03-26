// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Intl/DateTimeFormat
function formatDateIT(date: Date | undefined) {

  try {
    return date === undefined
      ? ""
      : new Intl.DateTimeFormat('it-IT', {
        //dateStyle: 'full',
        //timeStyle: 'short',
        //weekday: 'short',
        year: 'numeric',
        month: 'numeric',
        day: 'numeric',
        hour: 'numeric',
        minute: 'numeric',
        timeZone: "Europe/Rome"
      }).format(date);
  } catch (err) {
    return err as string
  }

}

export { formatDateIT };