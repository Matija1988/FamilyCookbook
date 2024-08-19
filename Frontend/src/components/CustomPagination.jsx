import Pagination from "react-bootstrap/Pagination";

export default function CustomPagination({ items }) {
  return (
    <>
      {items && items.lenght > 0 && (
        <div>
          <Pagination>
            <Pagination.First />
            <Pagination.Prev />
            <Pagination.Item> {items}</Pagination.Item>

            <Pagination.Next />
            <Pagination.Last />
          </Pagination>

          <br />
        </div>
      )}
    </>
  );
}
