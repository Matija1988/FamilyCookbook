import Pagination from "react-bootstrap/Pagination";

export default function CustomPagination({
  pageNumber,
  totalPages,
  handlePageChange,
  className,
}) {
  return (
    <>
      <div className={className}>
        <Pagination>
          <Pagination.First
            onClick={() => handlePageChange(1)}
            disabled={pageNumber === 1}
          />
          <Pagination.Prev
            onClick={() => handlePageChange(pageNumber - 1)}
            disabled={pageNumber === 1}
          />
          {[...Array(totalPages).keys()].map((page) => (
            <Pagination.Item
              key={page + 1}
              active={pageNumber === page + 1}
              onClick={() => handlePageChange(page + 1)}
            >
              {page + 1}
            </Pagination.Item>
          ))}
          <Pagination.Next
            onClick={() => handlePageChange(pageNumber + 1)}
            disabled={pageNumber === totalPages}
          />
          <Pagination.Last
            onClick={() => handlePageChange(totalPages)}
            disabled={pageNumber === totalPages}
          />
        </Pagination>
      </div>
    </>
  );
}
