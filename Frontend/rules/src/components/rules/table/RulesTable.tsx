import { Table } from "react-bootstrap";
import { type ReactNode } from "react";

interface RulesTableProps {
  children: ReactNode;
}

const RulesTable = ({ children }: RulesTableProps) => {
  return (
    <Table className="mt-2" striped responsive hover>
      <thead>
        <tr>
          <th scope="col">#</th>
          <th scope="col">Название</th>
          <th scope="col">Описание</th>
          <th scope="col">Уровень риска</th>
          <th scope="col">Статус</th>
          <th scope="col">Автор</th>
          <th scope="col">Создано</th>
          <th scope="col">Обновлено</th>
          <th scope="col" />
          <th scope="col" />
        </tr>
      </thead>
      <tbody>{children}</tbody>
    </Table>
  );
};

export default RulesTable;
