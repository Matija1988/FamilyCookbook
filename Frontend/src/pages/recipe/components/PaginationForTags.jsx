import Pagination from "react-bootstrap/Pagination";

export default function PaginationForTags({
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

          <Pagination.Item
            key={pageNumber}
            active={pageNumber + 1}
            onClick={() => handlePageChange(page + 1)}
          >
            {pageNumber}
          </Pagination.Item>
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
