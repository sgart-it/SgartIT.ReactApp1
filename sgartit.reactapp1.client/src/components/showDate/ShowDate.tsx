import { formatDateIT } from "../../helpers/formatDateIT";

type ShowDateProps = {
  date: Date | undefined;
}

export const ShowDate = ({ date }: ShowDateProps) => <div className="showDate">{formatDateIT(date)}</div>;
